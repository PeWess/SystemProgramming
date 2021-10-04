using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class SecondTask : MonoBehaviour
{
    private void Start()
    {
        TasksLauncher();
    }

    async void TasksLauncher()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        Task firstTask = new Task(() => TaskOne(cancellationToken));
        Task secondTask = new Task(() => TaskTwo(cancellationToken));
        firstTask.Start();
        secondTask.Start();

        await Task.WhenAll(firstTask, secondTask);
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    async Task TaskOne(CancellationToken cancellationToken)
    {
        Debug.Log("First task started");
        await Task.Delay(1000);
        Debug.Log("First task finished");
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }

    async Task TaskTwo(CancellationToken cancellationToken)
    {
        Debug.Log("Second task started");

        for (int i = 0; i <= 60; i++)
        {
            await Task.Yield();
        }
        Debug.Log("Second task finished");
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
}