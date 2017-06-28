namespace TPL
{
    using System;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        /// <summary>
        /// Waits for the task to complete, unwrapping any exceptions.
        /// Source: https://github.com/StephenCleary/AsyncEx.Tasks/blob/8beee56ac4fb095f0a1950ba84f2c25a2236683e/src/Nito.AsyncEx.Tasks/SynchronousTaskExtensions.cs
        /// </summary>
        /// <typeparam name="TResult">The type of the result of the task.</typeparam>
        /// <param name="task">The task. May not be <c>null</c>.</param>
        /// <returns>The result of the task.</returns>
        public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            return task.GetAwaiter().GetResult();
        }
    }
}
