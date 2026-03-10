using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp
{
    public static class TaskExtensionMethods
    {
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(state =>
            {
                ((TaskCompletionSource<object>)state).TrySetResult(null);
            }, tcs))
            {
                var tareaResultante = await Task.WhenAny(task, tcs.Task);

                if (tareaResultante == tcs.Task)
                {
                    throw new OperationCanceledException(cancellationToken);

                }
                else
                {
                    return await task;
                }
            }
        }
    }
}
