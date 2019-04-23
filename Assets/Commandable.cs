using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Commandable : MonoBehaviour
{

    public CommandInfo currentInfo;
    public Coroutine currentRoutine;
    public Coroutine previousRoutine = null;

    public void StateEnd()
    {
        previousRoutine = currentRoutine;

    }
    public void StateStart()
    {
        if (previousRoutine != null)
        {
            StopCoroutine(previousRoutine);
            previousRoutine = null;
        }
    }

    public abstract void Command(CommandInfo info);

    [System.Serializable]
    public struct CommandInfo
    {
        public Command command;
        public Vector3 point;
        public Transform target;
        

        public CommandInfo(Command command = Command.Nothing,Vector3 point = new Vector3(),Transform target = null)
        {
            this.command = command;
            this.point = point;
            this.target = target;
        }
        

        public enum Command
        {
            Nothing,
            Follow,
            Move,
            Attack,
            Interact,
            Defend
        }
    }
}

