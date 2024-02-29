//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.ApplicationModels;
//using Microsoft.AspNetCore.Mvc.Routing;
//using Microsoft.AspNetCore.Mvc;

//namespace IdentityService.Host
//{
//    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
//    {
//        public int Order => 3;

//        public void OnProvidersExecuted(ApplicationModelProviderContext context)
//        {
//        }

//        //public void OnProvidersExecuting(ApplicationModelProviderContext context)
//        //{
//        //    foreach (ControllerModel controller in context.Result.Controllers)
//        //    {
//        //        foreach (ActionModel action in controller.Actions)
//        //        {
//        //            // I assume that all you actions type are Task<ActionResult<ReturnType>>

//        //            //Type returnType = action.ActionMethod.ReturnType.GenericTypeArguments[0];//.GetGenericArguments()[0];

//        //            //action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status510NotExtended));
//        //            //action.Filters.Add(new ProducesResponseTypeAttribute(returnType, StatusCodes.Status200OK));
//        //            //action.Filters.Add(new ProducesResponseTypeAttribute(returnType, StatusCodes.Status500InternalServerError));


//        //            //action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status510NotExtended));
//        //            action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status201Created));
//        //            //action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
//        //        }
//        //    }
//        //}


//        public void OnProvidersExecuting(ApplicationModelProviderContext context)
//        {
//            // reference:
//            // https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/application-model?view=aspnetcore-7.0#iapplicationmodelprovider
//            // https://stackoverflow.com/questions/58047020/net-core-api-make-producesresponsetype-global-parameter-or-automate
//            foreach (ControllerModel controller in context.Result.Controllers)
//            {
//                foreach (ActionModel action in controller.Actions)
//                {
//                    Type? returnType = null;
//                    if (action.ActionMethod.ReturnType.GenericTypeArguments.Any())
//                    {
//                        if (action.ActionMethod.ReturnType.GenericTypeArguments[0].GetGenericArguments().Any())
//                        {
//                            returnType = action.ActionMethod.ReturnType.GenericTypeArguments[0].GetGenericArguments()[0];
//                        }
//                    }

//                    var method = action.Attributes.OfType<HttpMethodAttribute>().SelectMany(x => x.HttpMethods).FirstOrDefault();

//                    var hasAuthorize = action.Attributes.OfType<AuthorizeAttribute>().Any();

//                    var has200OK = action.Attributes.OfType<ProducesResponseTypeAttribute>()
//                        .Where(t => t.StatusCode == StatusCodes.Status200OK).Any();

//                    bool actionParametersExist = action.Parameters.Any();

//                    //if (actionParametersExist == true)
//                    //{
//                    //    AddProducesResponseTypeAttribute(action, null, 404);
//                    //}

//                    // 依据http method 添加对应的状态编码
//                    if (!string.IsNullOrWhiteSpace(method))
//                    {
//                        switch (method)
//                        {
//                            case "GET":
//                                AddGetStatusCodes(action, returnType);
//                                break;
//                            case "POST":
//                                if (!has200OK)
//                                {
//                                    AddPostStatusCodes(action, returnType, actionParametersExist);
//                                }
//                                break;
//                            case "PUT":
//                            case "DELETE":
//                                AddPutOrDeleteStatusCodes(action);
//                                break;
//                            default:
//                                break;
//                        }
//                    }

//                    if (hasAuthorize)
//                    {
//                        AddProducesResponseTypeAttribute(action, typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized);
//                        AddProducesResponseTypeAttribute(action, typeof(ApiErrorResponse), StatusCodes.Status403Forbidden);
//                    }

//                    // 通用状态编码
//                    AddUniversalStatusCodes(action, returnType);
//                }
//            }
//        }

//        public void AddProducesResponseTypeAttribute(ActionModel action, Type? returnType, int statusCodeResult)
//        {
//            if (returnType != null)
//            {
//                action.Filters.Add(new ProducesResponseTypeAttribute(returnType, statusCodeResult));
//            }
//            else if (returnType == null)
//            {
//                action.Filters.Add(new ProducesResponseTypeAttribute(statusCodeResult));
//            }
//        }

//        public void AddUniversalStatusCodes(ActionModel action, Type? returnType)
//        {
//            AddProducesResponseTypeAttribute(action, typeof(ApiErrorResponse), StatusCodes.Status400BadRequest);
//            AddProducesResponseTypeAttribute(action, typeof(ApiErrorResponse), StatusCodes.Status422UnprocessableEntity);
//            AddProducesResponseTypeAttribute(action, typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError);
//        }

//        public void AddGetStatusCodes(ActionModel action, Type? returnType)
//        {
//            AddProducesResponseTypeAttribute(action, returnType, StatusCodes.Status200OK);
//        }

//        public void AddPostStatusCodes(ActionModel action, Type? returnType, bool actionParametersExist)
//        {
//            AddProducesResponseTypeAttribute(action, returnType, StatusCodes.Status201Created);
//            //AddProducesResponseTypeAttribute(action, typeof(ApiErrorResponse), 400);
//            //if (actionParametersExist == false)
//            //{
//            //    AddProducesResponseTypeAttribute(action, null, 404);
//            //}
//        }

//        public void AddPutOrDeleteStatusCodes(ActionModel action)
//        {
//            AddProducesResponseTypeAttribute(action, null, 204);
//        }
//    }
//}
