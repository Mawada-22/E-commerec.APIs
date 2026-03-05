using Microsoft.AspNetCore.Mvc;
using Shared.Error_Models;
using System.Net;

namespace E_commerce.Apis.Factories
{
    public class ApiResponseFactory
    {
        //context => ModelState => dictionary <state, ModelEntery>
        //string => Key(Nameof parameter)
        //modelStateDictionary ===> Objects, Errors

        public static IActionResult CustomValidationErrorResponse(ActionContext actionContext)
        {
            //1- get all error on modelstate 
            var errors = actionContext.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidationError
                {
                    Feild = error.Key,
                    Erroes = error.Value.Errors.Select(e => e.ErrorMessage)

                });
            //2- create custom respone to retun 
            var response = new ValidationErrorResponse
            {
                stateCode = (int)HttpStatusCode.BadRequest,
                message = "ValidationError",
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        }
    }
}
