using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptGen.Classes.ISeq
{
    public class State
    {
        //haha jonathon
        //you will refer to yourself
        public string StateName { get; }
        public State PreviousState { get; }
        public string NextState { get; }
        public string Action { get; }
        public bool canBeLooped { get; }

        /// <summary>
        /// Normal State Definition
        /// </summary>
        /// <param name="previousState">The previous state in the state machine</param>
        /// <param name="nextState">The next state in the state machine</param>
        /// <param name="looping">Whether or not this state can refer to itself.</param>
        public State(string stateName, State previousState, string nextState, string action, bool looping)
        {
            StateName = stateName;
            PreviousState = previousState;
            NextState = nextState;
            canBeLooped = looping;
            Action = action;
        }

        /// <summary>
        /// Blank state. Should only be used to start a SM.
        /// </summary>
        public State()
        {

        }
    }
}
