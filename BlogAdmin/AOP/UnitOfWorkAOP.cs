using System;
using System.Linq;
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
                _unitOfWork.BeginTran();
                try
                {
                    invocation.Proceed();
                    //提交事务
                    _unitOfWork.CommitTran();
                }
                catch (Exception)
                {
                    //回滚
                    _unitOfWork.RollbackTran();
                    throw;
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
