using System;

public static class TaskUtils
{
    public static async System.Threading.Tasks.Task WaitUntil(Func<bool> predicate, int sleep = 50)
    {
        while (!predicate())
        {
            await System.Threading.Tasks.Task.Delay(sleep);
        }
    }
}
