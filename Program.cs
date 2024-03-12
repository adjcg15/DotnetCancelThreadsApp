namespace AppConHilos2;

class Program
{
    static void Main(string[] args)
    {
        Thread currentThread = Thread.CurrentThread;
        currentThread.Name = "Hilo principal";
        currentThread.Priority = ThreadPriority.Lowest;
        currentThread.IsBackground = false;

        Thread workerThread = new Thread(new ParameterizedThreadStart(Print));
        workerThread.Name = "Hilo de print";
        CancellationTokenSource cts = new CancellationTokenSource();
        workerThread.Start(cts.Token);

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Principal thread: {i}");
            Thread.Sleep(200);
        }

        if(workerThread.IsAlive) {
            cts.Cancel();
        }
    }

    static void Print(object? obj) {
        if(obj == null) {
            return;
        }

        CancellationToken token = (CancellationToken)obj;
        Thread currentThread = Thread.CurrentThread;
        currentThread.Priority = ThreadPriority.Highest;
        currentThread.IsBackground = false;

        for (int i = 11; i < 20; i++)
        {
            if(token.IsCancellationRequested) {
                Console.WriteLine($"En la iteración {i} la ejecución ha sido cancelada");
                break;
            }
            Console.WriteLine($"Principal thread: {i}");
            Thread.Sleep(1000);
        }
    }
}
