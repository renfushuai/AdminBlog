using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blog.Common;
using Blog.IRepository.UnitOfWork;
using Castle.DynamicProxy;
namespace BlogAdmin.AOP
{
    public class UnitOfWorkAOP : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkAOP(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var transactionAtt = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(UnitOfWorkAttribute));
            if (transactionAtt is UnitOfWorkAttribute)
            {
                try
                {
                    Console.WriteLine($"Begin Transaction");

                    _unitOfWork.BeginTran();

                    invocation.Proceed();


                    // 异步获取异常，先执行
                    if (InternalAsyncHelper.IsAsyncMethod(invocation.Method))
                    {
                        if (invocation.Method.ReturnType == typeof(Task))
                        {
                            invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                                (Task)invocation.ReturnValue,
                                ex =>
                                {
                                    _unitOfWork.RollbackTran();

                                });
                        }
                        else //Task<TResult>
                        {
                            invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                             invocation.Method.ReturnType.GenericTypeArguments[0],
                             invocation.ReturnValue,
                             ex =>
                             {
                                 _unitOfWork.RollbackTran();

                             });

                        }

                    }
                    _unitOfWork.CommitTran();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    _unitOfWork.RollbackTran();
                }

            }
            else
            {
                //如果没有标记[UnitOfWork]，直接执行方法
                invocation.Proceed();
            }
        }



       
    }

}
