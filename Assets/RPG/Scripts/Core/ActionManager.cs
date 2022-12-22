/*
 * This script is a component for a Unity game object that manages the
 * execution of actions. It has a property called CurrentAction of type
 * IAction, which represents the current action being performed. The IAction
 * interface represents a class that can perform some action.
 *  
 *  The script has two methods:
 *  
 *  1. StartAction(IAction action): This method takes an IAction object as a
 * parameter and sets it as the current action. If the same action is passed
 * in again, the method does nothing and returns early. If a different action
 * is passed in, the method cancels the current action by calling its Cancel()
 * method and then sets the new action as the current action.
 *  
 *  2. CancelAction(): This method cancels the current action by calling
 * StartAction(null).
 *  
 * The ActionManager component can be attached to a game object and used to
 * control the execution of actions in the game. When an action needs to be
 * performed, it can be passed to the StartAction() method, which will take
 * care of cancelling any existing action and setting the new action as the
 * current action. When the action is complete or needs to be interrupted,
 * the CancelAction() method can be called to cancel the current action.
 */

using UnityEngine;

namespace RPG.Core
{
    public class ActionManager : MonoBehaviour
    {
        // The current action being performed.
        private IAction CurrentAction { get; set; }

        /// <summary>
        /// Starts the specified action.
        /// 
        /// If the same action is passed in again, this method does nothing
        /// and returns early.
        /// 
        /// If a different action is passed in, the current action is
        /// cancelled and the new action is set as the current action.
        /// </summary>
        /// <param name="action">The action to start.</param>
        public void StartAction(IAction action)
        {
            // If the same action is passed in, do nothing and return early.
            if (CurrentAction == action) { return; }

            // Cancel the current action.
            CurrentAction?.Cancel();

            // Set the new action as the current action.
            CurrentAction = action;
        }

        /// <summary>
        /// Cancels the current action.
        /// </summary>
        public void CancelAction()
        {
            StartAction(null);
        }
    }
}
