namespace TPL
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using System.Windows;

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Producer
        private async Task ProduceAsync(ITargetBlock<int> queue, CancellationToken cancellationToken)
        {
            const int count = 100;

            try
            {
                for (int i = 0; i < count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // produce
                    int value = i + 1;

                    // variant 1: On UI thread
                    await queue.SendAsync(value);

                    // variant 2: on thread pool thread
                    // todo : Code change needed in StartButton_Click
                    //new TaskExecutor().Run(() => queue.SendAsync(value)).WaitAndUnwrapException();

                    listBox.Items.Add(string.Format("Produced: {0}", value));
                }
            }
            catch (TaskCanceledException ex)  // OperationCanceledException
            {
                listBox.Items.Add(string.Format("ReceiveSubstanceItems canceled: {0}", ex));
            }
            finally
            {
                queue.Complete();
            }
        }

        // Consumer
        private async Task ConsumeAsync(ISourceBlock<int> queue, CancellationToken cancellationToken)
        {
            while (await queue.OutputAvailableAsync())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    listBox.Items.Add("updating substance items cancelled");
                    break;
                }

                int value = await queue.ReceiveAsync();

                listBox.Items.Add(string.Format("Consumed: {0}", value));
                await Task.Delay(250);
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            DataflowBlockOptions options = new DataflowBlockOptions { BoundedCapacity = 10 };
            BufferBlock<int> queue = new BufferBlock<int>(options);

            Task p = ConsumeAsync(queue, cancellationToken);
            Task c = ProduceAsync(queue, cancellationToken);

            try
            {
                await Task.WhenAll(p, c);
            }
            catch (OperationCanceledException ex)
            {
                listBox.Items.Add(string.Format("Start operation canceled: {0}", ex));
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();
        }
    }
}
