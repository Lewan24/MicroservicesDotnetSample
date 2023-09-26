# MicroservicesDotnetSample

Application is created with Blazor WebAssembly and microservices as .net web apis.

Main webapi gateway is redirecting specified requests to matched microservices.
This type of architecture if for full fotnet applications, or when you want firstly check authorization of user, and then send request.
Microservices does not have any auth methods or validation. Everything is happening in gateway before sending request.

This sample is mainly for my learning and testing purposes. I need any specific solutions for my private projects. And that's the one of them.

For now the gateway handles only HttpGet methods, but its like just copy get redirect and change to HttpPost.

## Logic scheme and achritecture

![Project scheme](https://github.com/Lewan24/MicroservicesDotnetSample/blob/master/Architecture_v1.png)
