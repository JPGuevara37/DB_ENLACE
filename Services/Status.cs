using System.Net.Mail;
using DB_Enlace.models;
using Microsoft.EntityFrameworkCore.Internal;

using webapi.Services;

namespace webapi.Services
{
    public class ExampleService : IExampleService
    {
        public IResponse GetSuccessfulExample()
        {
            // Implementar la lógica para obtener un ejemplo exitoso desde el backend
            return new ApiResponse
            {
                result = new { token = "tu_token_generado", error_msj = "" }
            };
        }

        public IResponse GetErrorExample()
        {
            // Implementar la lógica para obtener un ejemplo con error desde el backend
            return new ApiResponse
            {
                status = "error",
                result = new { token = "", error_msj = "Mensaje de error" }
            };
        }
    }
}

namespace webapi.Services
{
    public class ApiResponse : IResponse
    {
        public string status { get; set; } = "OK";
        public object result { get; set; }
    }
}


public interface IExampleService
{
    IResponse GetSuccessfulExample();
    IResponse GetErrorExample();
}
    public interface IResponse
    {
        string status { get; set; }
        object result { get; set; }
    }
