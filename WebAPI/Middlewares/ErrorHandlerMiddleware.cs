﻿using Application.Execteptions;
using Application.Execteptions.Validation;
using Application.Whappers;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new Response<string>() 
                { Succeeded = false, Message = error?.Message};
                switch (error)
                {
                    case ApiException exception:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ValidationException exception:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel.Errors = exception.Errors;
                        break;
                    case KeyNotFoundException exception:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
                }
            }
            
        }
    }

