﻿using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.SimpleInjector
{
    public class StatsDCountingInterceptor : SimpleInjectorInterceptor<StatsDCounting>, IInterceptor
    {
        public StatsDCountingInterceptor(IVeStatsDClient client) : base(client)
        {
        }

        protected override void Invoke(IInvocation invocation, StatsDCounting attr)
        {
            invocation.Proceed();

            var name = attr.Name;
            var methodBase = invocation.GetConcreteMethod();
            var method = methodBase.Name;
            var target = invocation.InvocationTarget.GetType().Name;

            name = name.Replace("{type}", target.ToLowerInvariant());
            name = name.Replace("{method}", method.ToLowerInvariant());

            if (methodBase.IsGenericMethod)
            {
                name = GetGenericName(name, methodBase);
            }

            Client.LogCount(name, attr.Count, attr.Tags);
        }
    }
}
