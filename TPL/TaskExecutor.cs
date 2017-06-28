namespace TPL
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskExecutor // : ITaskExecutor
    {
        private const TaskCreationOptions LongRunningTaskCreationOptions = TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning;

        public Task Run(Action action)
        {
            Task task = Task.Run(action);
            LogTask(task);
            return task;
        }

        public Task Run(Action action, CancellationToken cancellationToken)
        {
            Task task = Task.Run(action, cancellationToken);
            LogTask(task);
            return task;
        }

        public Task Run(Func<Task> function)
        {
            Task task = Task.Run(function);
            LogTask(task);
            return task;
        }

        public Task Run(Func<Task> function, CancellationToken cancellationToken)
        {
            Task task = Task.Run(function, cancellationToken);
            LogTask(task);
            return task;
        }

        public Task<TResult> Run<TResult>(Func<TResult> function)
        {
            Task<TResult> task = Task.Run(function);
            LogTask(task);
            return task;
        }

        public Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken)
        {
            Task<TResult> task = Task.Run(function, cancellationToken);
            LogTask(task);
            return task;
        }

        public Task<TResult> Run<TResult>(Func<Task<TResult>> function)
        {
            Task<TResult> task = Task.Run(function);
            LogTask(task);
            return task;
        }

        public Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken)
        {
            Task<TResult> task = Task.Run(function, cancellationToken);
            LogTask(task);
            return task;
        }

        public Task StartNewLongRunning(Action action)
        {
            Task task = Task.Factory.StartNew(action, CancellationToken.None, LongRunningTaskCreationOptions, TaskScheduler.Default);
            LogTask(task);
            return task;
        }

        public Task StartNewLongRunning(Action action, CancellationToken cancellationToken)
        {
            Task task = Task.Factory.StartNew(action, cancellationToken, LongRunningTaskCreationOptions, TaskScheduler.Default);
            LogTask(task);
            return task;
        }

        private void LogTask(Task task)
        {
            System.Diagnostics.Debug.WriteLine("Task '{0}' queued for execution", task.Id);
        }
    }
}