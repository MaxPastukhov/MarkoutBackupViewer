using System;
using System.ComponentModel;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// запуск задач в фоне
    /// </summary>
    public static class Background
    {
        /// <summary>
        /// void Action();
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        public static void Run(Action action, Action<Exception> callback)
        {
            new Worker
            {
                Action = action,
                Report = callback
            }.RunWorkerAsync();
        }

        /// <summary>
        /// T Action();
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        public static ICancellable Run(Action<ICancellable> action, Action<Exception> callback)
        {
            Worker worker = new Worker
            {
                Report = callback
            };
            worker.Action = () => action(worker);
            worker.RunWorkerAsync();
            return worker;
        }

        /// <summary>
        /// T Action();
        /// </summary>
        /// <param name="action"></param>
        /// <param name="callback"></param>
        public static void Run<T>(Func<T> action, Action<Exception, T> callback)
        {
            T result = default (T);
            new Worker
            {
                Action = () => result = action(),
                Report = exception => callback(exception, result)
            }.RunWorkerAsync();
        }

        /// <summary>
        /// воркер
        /// </summary>
        private class Worker : BackgroundWorker, ICancellable
        {
            /// <summary>
            /// конструктор
            /// </summary>
            public Worker()
            {
                WorkerSupportsCancellation = true;
                DoWork += Worker_DoWork;
                RunWorkerCompleted += Worker_RunWorkerCompleted;
            }

            /// <summary>
            /// исполнение задачи
            /// </summary>
            public Action Action;

            /// <summary>
            /// отчет об исполненной задаче
            /// </summary>
            public Action<Exception> Report;

            /// <summary>
            /// ошибка, возникшая в процесс исполнения
            /// </summary>
            private Exception Exception;

            /// <summary>
            /// исполнение задачи
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Worker_DoWork(object sender, DoWorkEventArgs e)
            {
                // выполним
                try
                {
                    Action();
                }
                catch (Exception exception)
                {
                    Exception = exception;
                }
            }

            /// <summary>
            /// работа завершена
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                Report(Exception);
            }

            /// <summary>
            /// отмена задачи
            /// </summary>
            public void Cancel()
            {
                CancelAsync();
            }

            /// <summary>
            /// задача отменена?
            /// </summary>
            public bool Cancelled
            {
                get { return CancellationPending; }
            }
        }

        /// <summary>
        /// отменяемая задача
        /// </summary>
        public interface ICancellable
        {
            /// <summary>
            /// отменить
            /// </summary>
            void Cancel();

            /// <summary>
            /// отменена ли задача?
            /// </summary>
            bool Cancelled { get; }
        }
    }
}
