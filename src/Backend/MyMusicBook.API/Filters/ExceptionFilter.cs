using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyMusicBook.Communication.Responses;
using MyMusicBook.Exceptions;
using MyMusicBook.Exceptions.ExceptionsBase;
using System;
using System.Net;

namespace MyMusicBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyMusicBookException)
            HandleProjectException(context);
        else
            ThrowUnknownException(context);
    }

    private void HandleProjectException(ExceptionContext context)
    {
        if(context.Exception is ErrorOnValidationException)
        {
            var exception = context.Exception as ErrorOnValidationException;

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
        }
    }
    private void ThrowUnknownException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }

}
