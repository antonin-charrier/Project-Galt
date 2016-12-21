using Microsoft.AspNetCore.Mvc;
using System;

namespace Galt.Controllers
{
    public class ActionResultOptions<T, TViewModel>
    {
        public ActionResultOptions( Controller controller )
        {
            if ( controller == null ) throw new ArgumentNullException( nameof( controller ) );
            Controller = controller;
        }

        public Func<T, TViewModel> ToViewModel { get; set; }

        public string RouteName { get; set; }

        public Func<T, object> RouteValues { get; set; }

        public Controller Controller { get; }
    }
}
