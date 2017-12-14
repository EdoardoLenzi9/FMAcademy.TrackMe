using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FactoryMind.TrackMe.Business.Exceptions;
using FactoryMind.TrackMe.Business.Services;
using FactoryMind.TrackMe.Domain.Models;
using FactoryMind.TrackMe.Server.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate next;
    private AuthenticationService _authenticationService;

    public AuthorizationMiddleware(RequestDelegate next, AuthenticationService authenticationService)
    {
        this.next = next;
        _authenticationService = authenticationService;
    }

    public async Task Invoke(HttpContext context, IAuthorizationContext authorizationContext)
    {
        List<string> freePath = new List<string>();
        //freePath.Add("/index");
        //freePath.Add("/favicon.ico");
        bool flag = true; // true per togliere middleware
        //bool flag = true;
        Console.WriteLine($"\n\nRichiesta Route: {context.Request.Path}\nId utente :[{context.Request.Headers["id"]}]\n\n");
        //Console.WriteLine("\n\n"+context.Request.Body.CanRead+ "\n\n");
        if(freePath.Contains(context.Request.Path)){
            flag = true;
        }
        if (context.Request.Headers.Keys.Contains("id"))
        {
            flag = await _authenticationService.isUserRegistredAsync(int.Parse(context.Request.Headers["id"]));
            if(flag)
            {
                var authorizationCtx = authorizationContext as AuthorizationContext;
                authorizationCtx.User = await _authenticationService.GetUserByIdAsync(int.Parse(context.Request.Headers["id"]));
            }
        }
        if (context.Request.Headers.Keys.Contains("mail"))
        {
            flag = true;
        }
        if (!flag)
        {
            throw new AuthorizationException("Authorization Middleware Fail");
        }
        await next(context);
    }
}
