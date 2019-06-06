using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSvc : ServiceRoot<TimerSvc>
{ 
    private int uniqueId;
    private List<int> uniqueIdList = new List<int>();
     
    private List<TimeTask> tmpTaskList = new List<TimeTask>();
    private List<TimeTask> taskList = new List<TimeTask>();
     
    private int frameCounter;
    private List<FrameTask> tmpFrameTaskList = new List<FrameTask>();
    private List<FrameTask> frameTaskList = new List<FrameTask>(); 

    private void Update()
    {
        Tick();
    }

    public void Tick() {
        frameCounter++;
        TickTimeTask();
        TimeFrameTask();
    }
     
    #region  计时任务
    private void TickTimeTask() {
        for (int i = 0; i < tmpTaskList.Count; i++)
        {
            taskList.Add(tmpTaskList[i]);
        }
        tmpTaskList.Clear();
         

        for (int i = 0; i < taskList.Count; i++)
        {
            TimeTask task = taskList[i]; 
            if (task.count == 0 || task.count < -1) {
                RemoveUniqueId(task.taskId);
                taskList.RemoveAt(i);
                i -= 1;
                continue;
            }
            if (Time.realtimeSinceStartup * 1000 >= task.destTime)  
            { 
                try
                {
                    if (task.taskCB != null) {
                        task.taskCB();
                    }
                    if (task.count != -1)
                    {
                        task.count -= 1;
                        if (task.count == 0)
                        {
                            RemoveUniqueId(task.taskId);
                            taskList.RemoveAt(i);
                            i -= 1;
                        }
                        else {
                            task.destTime = Time.realtimeSinceStartup * 1000 + task.intervalTime;
                        }
                    }
                    else { 
                        task.destTime = Time.realtimeSinceStartup * 1000 + task.intervalTime;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            else {
                continue;
            }
        }

    } 

    /// <summary>
    ///  添加一个定时任务
    /// </summary> 
    /// <returns>任务的唯一id</returns>
    public int AddTimeTask(Action cb,float interval,int count = 1, TimeUnit tUnit = TimeUnit.Millisecond) {
        if (tUnit != TimeUnit.Millisecond) {
            interval = GetMilliSecond(interval, tUnit);
        }
        int id = GetUniqueId();
        TimeTask task = new TimeTask(id, cb, Time.realtimeSinceStartup * 1000 + interval, interval, count); 
        tmpTaskList.Add(task);
        uniqueIdList.Add(id);

        return id;
    }

    public bool DeleteTimeTask(int taskId) {
        bool ret = false;
        for (int i = 0; i < taskList.Count; i++)
        {
            if (taskId == taskList[i].taskId) {
                taskList.RemoveAt(i);
                ret = true;
                break;
            }
        }
        RemoveUniqueId(taskId);

        if (ret == false) {
            for (int i = 0; i < tmpTaskList.Count; i++)
            {
                if (taskId == tmpTaskList[i].taskId)
                {
                    tmpTaskList.RemoveAt(i);
                    ret = true;
                    break;
                }
            }
        }
        return ret;
    }


    public bool ReplaceTimeTask(int taskid, Action cb, float interval, int count = 1, TimeUnit tUnit = TimeUnit.Millisecond) {
        bool canfindTask = false;
        TimeTask task = null;

        for (int i = 0; i < taskList.Count; i++)
        {
            if (taskid == taskList[i].taskId) {
                task = taskList[i];
                canfindTask = true;
            }
        }
        if (canfindTask == false) {
            for (int i = 0; i < tmpTaskList.Count; i++)
            {
                if (taskid == tmpTaskList[i].taskId)
                {
                    task = tmpTaskList[i];
                    canfindTask = true;
                }
            }
        }

        if (canfindTask && task != null) {
            if (tUnit != TimeUnit.Millisecond)
            {
                interval = GetMilliSecond(interval, tUnit);
            }

            task.taskCB = cb;
            task.destTime = Time.realtimeSinceStartup * 1000 + interval;
            task.intervalTime = interval;
            task.count = count;
        }
        return canfindTask;
    }

    #endregion


    #region 帧任务
    private void TimeFrameTask() {
        frameCounter++;
        for (int i = 0; i < tmpFrameTaskList.Count; i++)
        {
            frameTaskList.Add(tmpFrameTaskList[i]);
        }
        tmpFrameTaskList.Clear();
        for (int i = 0; i < frameTaskList.Count; i++)
        {
            FrameTask task = frameTaskList[i];
            if (task.count == 0 || task.count < -1) {
                RemoveUniqueId(task.taskId);
                frameTaskList.RemoveAt(i);
                i -= 1;
                continue;
            }
            if (frameCounter >= task.destFrame)
            {
                if (task.count != -1)
                {
                    task.count -= 1;
                    if (task.count == 0)
                    {
                        RemoveUniqueId(task.taskId);
                        frameTaskList.RemoveAt(i);
                        i -= 1;
                    }
                    else {
                        task.destFrame = frameCounter + task.intervalFrame;
                    }
                }
                else {
                    task.destFrame = frameCounter + task.intervalFrame;
                }
            }
            else {
                continue;
            }
        }
    }
     
    public int AddFrameTask(Action cb, int interval, int count = 1) {
        int id = GetUniqueId();
        FrameTask task = new FrameTask(id, cb, frameCounter + interval, interval, count);
        tmpFrameTaskList.Add(task);
        uniqueIdList.Add(id);

        return id;
    }

    public bool DeleteFrameTask(int taskid) {
        bool ret = false;

        for (int i = 0; i < frameTaskList.Count; i++)
        {
            if (taskid == frameTaskList[i].taskId) {
                frameTaskList.RemoveAt(i);
                ret = true;
                break;
            }
        }
        RemoveUniqueId(taskid);
        if (!ret)
        {
            for (int i = 0; i < tmpFrameTaskList.Count; i++)
            {
                if (taskid == tmpFrameTaskList[i].taskId) {
                    tmpFrameTaskList.RemoveAt(i);
                    ret = true;
                    break;
                }
            }
        }
        return ret;
    }

    public bool ReplaceFrameTask(int id,Action cb,int interval,int count = 1) {
        bool canfindTask = false;
        FrameTask task = null;

        for (int i = 0; i < frameTaskList.Count; i++)
        {
            if (id == frameTaskList[i].taskId)
            {
                task = frameTaskList[i];
                canfindTask = true;
                break;
            }
        } 
        if (!canfindTask)
        {
            for (int i = 0; i < tmpFrameTaskList.Count; i++)
            {
                if (id == tmpFrameTaskList[i].taskId)
                {
                    task = tmpFrameTaskList[i];
                    canfindTask = true;
                    break;
                }
            }
        }

        if (canfindTask && task != null) {
            task.taskCb = cb;
            task.destFrame = frameCounter + interval;
            task.intervalFrame = interval;
            task.count = count;
        }

        return canfindTask;
    }
   


    #endregion 

    public bool RemoveUniqueId(int taskId) {
        bool ret = false;
        for (int i = 0; i < uniqueIdList.Count; i++)
        {
            if (taskId == uniqueIdList[i])
            {
                uniqueIdList.RemoveAt(i);
                ret = true;
                break;
            }
        }

        return ret;
    }  

    public int GetUniqueId() {
        uniqueId += 1;
        if (uniqueId >= int.MaxValue) {
            while (true)
            {
                bool used = false;
                for (int i = 0; i < uniqueIdList.Count; i++)
                {
                    if (uniqueId == uniqueIdList[i]) {
                        used = true;
                        break;
                    }
                }
                if (!used)
                {
                    break;
                }
                else {
                    uniqueId += 1;
                }
            }
        }

        return uniqueId;
    }


    private float GetMilliSecond(float value, TimeUnit _unit) {
        switch (_unit)
        {
            case TimeUnit.Millisecond:
                return value;
            case TimeUnit.Second:
                return value * 1000;
            case TimeUnit.Minute:
                return value * 1000 * 60;
            case TimeUnit.Hour:
                return value * 1000 * 60 * 60;
            case TimeUnit.Day:
                return value * 1000 * 60 * 60 * 24;
            default:
                return value; 
        }
    }
}

public class TimeTask
{
    //全局唯一的任务id
    public int taskId;
    public Action taskCB;
    //目标执行时间
    public float destTime;
    //间隔时间
    public float intervalTime;
    //执行次数
    public int count;

    public TimeTask(int id, Action cb, float destT, float interval, int count)
    {
        this.taskId = id;
        this.taskCB = cb;
        this.destTime = destT;
        this.intervalTime = interval;
        this.count = count;
    }
}


public class FrameTask
{
    public int taskId;
    public Action taskCb;
    public int destFrame;
    public int intervalFrame;
    public int count;

    public FrameTask(int id, Action cb, int destF, int interval, int count)
    {
        this.taskId = id;
        this.taskCb = cb;
        this.destFrame = destF;
        this.intervalFrame = interval;
        this.count = count;
    }
}


/// <summary>
/// 时间单位
/// </summary>
public enum TimeUnit
{
    Millisecond,
    Second,
    Minute,
    Hour,
    Day,
}
