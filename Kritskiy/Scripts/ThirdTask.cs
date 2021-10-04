using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ThirdTask : MonoBehaviour
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

        await WhatTaskFasterAsync(cancellationToken, firstTask, secondTask);
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    async Task TaskOne(CancellationToken cancellationToken)
    {
        Debug.Log("First task started");
        await Task.Delay(1000);

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        Debug.Log("First task finished");
    }

    async Task TaskTwo(CancellationToken cancellationToken)
    {
        Debug.Log("Second task started");

        for (int i = 0; i <= 60; i++)
        {
            await Task.Yield();
        }

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        Debug.Log("Second task finished");
    }

    public static async Task<bool> WhatTaskFasterAsync(CancellationToken cancellationToken, Task taskOne, Task taskTwo)
    {
        var answer = await Task.WhenAny(taskOne, taskTwo);
        if (answer == taskOne)
        {
            Debug.Log("Task one is faster.");
            return true;
        }
        else if (answer == taskTwo)
        {
            Debug.Log("Task two is faster.");
            return false;
        }
        else if (cancellationToken.IsCancellationRequested)
        {
            return false;
        }

        return false;
    }
}