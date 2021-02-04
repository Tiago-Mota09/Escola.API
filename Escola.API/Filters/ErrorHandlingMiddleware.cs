using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using Signa.Library.Exceptions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Angis.CTe.API.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private static IExceptionHandling _exceptionHandling;

        public IConfiguration Configuration { get; }

        public ErrorHandlingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.Configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        //https://stackoverflow.com/questions/29664/how-to-catch-sqlserver-timeout-exceptions
        private enum SQLError
        {
            NetworkError = 11,
            OutOfMemory = 701,
            LockIssue = 1204,
            DeadLock = 1205,
            LockTimeout = 1222
        }

        class ErrorHandlingModel
        {
            public int ErrorCode { get; set; }
            public object ErrorObject { get; set; }

            public ErrorHandlingModel()
            {
                ErrorCode = (int)HttpStatusCode.InternalServerError;
                ErrorObject = null;
            }

            public ErrorHandlingModel(int errorCode, object ErrorObject)
            {
                ErrorCode = errorCode;
                this.ErrorObject = ErrorObject;
            }
        }

        interface IExceptionHandling { ErrorHandlingModel TratarErro(Exception ex); }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            try
            {
                _exceptionHandling = VerificarTipoExcecao(exception, context);
                var errorHandling = _exceptionHandling.TratarErro(exception);
                context.Response.StatusCode = errorHandling.ErrorCode;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(JsonConvert.SerializeObject(errorHandling.ErrorObject));
            }
            catch (Exception ex)
            {
                return context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    Message = "Problemas no sistema. Entre em contato com o administrador do sistema.",
                    Stack = ex.ToString()
                }));
            }
        }

        private static IExceptionHandling VerificarTipoExcecao(Exception ex, HttpContext context)
        {
            if (ex is SignaRegraNegocioException) { return new SignaBusinessHandling(); }
            if (ex is SignaSqlNotFoundException) { return new SignaSqlNotFoundHandling(); }
            if (ex is NpgsqlException) { return new SqlHandling(context); }
            return new GenericHandling();
        }

        class SignaBusinessHandling : IExceptionHandling
        {
            //private LogDatabaseDAO _logDatabase;

            //public SignaBusinessHandling(LogDatabaseDAO logDatabase)
            //{
            //    _logDatabase = logDatabase;
            //}

            public SignaBusinessHandling()
            {

            }

            public ErrorHandlingModel TratarErro(Exception ex)
            {
                var exception = ex as SignaRegraNegocioException;
                var errorHandlingModel = (exception == null) ? new ErrorHandlingModel() :
                                          new ErrorHandlingModel((int)HttpStatusCode.BadRequest, new { ex.Message });

                //var userId = (Globals.UserId.HasValue) ? Globals.UserId.Value : Globals.UserId;
                //var companyId = (Globals.CompanyId.HasValue) ? Globals.CompanyId.Value : Globals.CompanyId;

                //var log = new LogMsg()
                //{
                //    UsuarioId = userId,
                //    EmpresaId = companyId,
                //    TabTipoMsgId = 2,
                //    MensagemUsuario = exception.Message,
                //    MensagemTecnica = null,
                //    StackTrace = ex.StackTrace.ToString()
                //};

                //_logDatabase.Insert(log);

                return errorHandlingModel;
            }
        }

        class SignaSqlNotFoundHandling : IExceptionHandling
        {
            public ErrorHandlingModel TratarErro(Exception ex)
            {
                var exception = ex as SignaSqlNotFoundException;
                var errorHandlingModel = (exception == null) ? new ErrorHandlingModel() :
                                          new ErrorHandlingModel((int)HttpStatusCode.NotFound, new { ex.Message });
                return errorHandlingModel;
            }
        }

        class SqlHandling : IExceptionHandling
        {
            //private DatabaseLogHelper _databaseLog;
            private HttpContext _context;

            //public SqlHandling(DatabaseLogHelper databaseLog, HttpContext context)
            //{
            //    _databaseLog = databaseLog;
            //    _context = context;
            //}

            public SqlHandling( HttpContext context)
            {
                _context = context;
            }

            public ErrorHandlingModel TratarErro(Exception ex)
            {
                string mensagemUsuario = null;

                if (ex is PostgresException)
                {
                    var sqlEx = ex as PostgresException;

                    int.TryParse(sqlEx.SqlState, out int code);

                    //TODO - Mapear códigos de erro para o enum SQLError
                    if (Enum.IsDefined(typeof(SQLError), code) ||
                       (code == -2 && Regex.IsMatch(ex.Message, @"[tT]ime\-{0,1}out")))
                    {
                        mensagemUsuario = "Tivemos uma instabilidade temporária.\nTente novamente e, se não funcionar, informe ao nosso suporte";
                    }
                    else if (sqlEx.ErrorCode >= 50000)//Mensagens do usuário - Revisar no PgSQL
                    {
                        //mensagemUsuario = sqlEx.Errors[0].Message;
                    }
                    else if (sqlEx.ErrorCode == 4060)//Conexão com a base - Revisar no PgSQL
                    {
                        mensagemUsuario = $"Não foi possível conectar ao server {sqlEx.Routine}; {sqlEx.Message}";
                    }
                    else
                    {
                        var methodName = new StackTrace(sqlEx, true).GetFrames()
                                                                    .Where(f => f.GetMethod().DeclaringType
                                                                                 .FullName.Contains("Data.Repository"))
                                                                                 .FirstOrDefault().GetMethod();

                        mensagemUsuario = $"Não foi possível completar a operação. {sqlEx.MessageText} {methodName.DeclaringType.FullName}.{methodName.Name}";
                    }
                }
                else
                {
                    mensagemUsuario = "Problemas na nossa base de dados. Informe o suporte.";
                }

                //_databaseLog.GravaLogMsg(mensagemUsuario, ex, _context);

                return new ErrorHandlingModel((int)HttpStatusCode.InternalServerError,
                                              new { Message = mensagemUsuario, stack = ex.ToString() });
            }
        }

        class GenericHandling : IExceptionHandling
        {
            // private LogDatabaseDAO _logDatabase;

            //public GenericHandling(LogDatabaseDAO logDatabase)
            //{
            //    _logDatabase = logDatabase;
            //}

            public GenericHandling()
            {

            }

            public ErrorHandlingModel TratarErro(Exception ex)
            {
                var frame = new StackFrame();

                try
                {
                    frame = new StackTrace(ex, true)
                                 .GetFrames()
                                 .First(f => f.GetMethod().DeclaringType.Name.Contains("Data.Repository") ||
                                        f.GetMethod().DeclaringType.Name.Contains("DataStopCommand") ||
                                        f.GetMethod().DeclaringType.Name.Contains("String") ||
                                        f.GetMethod().DeclaringType.Name.Contains("DateTime"));
                }
                catch (Exception)
                {
                    frame = new StackTrace(ex, true).GetFrames().FirstOrDefault();
                }

                var result = JsonConvert.SerializeObject(new
                {
                    Message = "Problemas no sistema. Entre em contato com o administrador do sistema.",
                    Error = new
                    {
                        Text = ex.Message,
                        Method = (frame.GetMethod().DeclaringType == null ? "" : frame.GetMethod().DeclaringType.Name) +
                                 "." + frame.GetMethod().Name,
                        Line = frame.GetFileLineNumber(),
                        Column = frame.GetFileColumnNumber() + "." + (int)HttpStatusCode.BadRequest
                    }
                });

                return new ErrorHandlingModel((int)HttpStatusCode.BadRequest, result);
            }
        }
    }
}
