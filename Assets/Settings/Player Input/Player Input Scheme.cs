//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Settings/Player Input/Player Input Scheme.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputScheme : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputScheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input Scheme"",
    ""maps"": [
        {
            ""name"": ""Player Movement"",
            ""id"": ""6aa0d5c0-4475-4e5f-b282-08cbf452caa3"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d0fc48a8-5002-4561-b779-7b30064f1630"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""PassThrough"",
                    ""id"": ""dd600bb6-18c3-4df9-bb8c-175cca0e05ee"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""05d611be-4b5d-4969-80b2-877b48e2f47b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""84436fef-45ce-430f-8e38-e726bef5c778"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c9cd342c-8553-4374-aa8f-7b4c7d9b0b3c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1fb71c1c-29e8-4801-8744-5ff75d50124f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""eed05d95-51d3-4c00-ac5d-dc73d7360770"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0f42ce8f-a08c-4807-9b87-212fe18342bf"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""86ae703d-e3db-4f5f-a495-e27413e0dee7"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.03,y=0.03)"",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5cccb757-3fc4-44cb-9e80-0c41466146d8"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Actions"",
            ""id"": ""402adc07-4733-439e-bec2-cd2ef3ab31ec"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""45864042-1e00-41b6-8150-f7867aee4963"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""0fb7e9cd-2de9-45a5-b5f1-24abcb2163df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Block"",
                    ""type"": ""Button"",
                    ""id"": ""e1b8fcb9-0467-4297-ae2b-8905877911c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Focus On Enemy"",
                    ""type"": ""Button"",
                    ""id"": ""e004e46e-4011-43d7-bf5b-b525bc2aa54f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""641ddc74-2493-4aa9-b71e-a46856bb1605"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b230fc5-c632-4ffe-be16-05f9c44d43b5"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Focus On Enemy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50f5d8cb-12e5-450c-bb59-c58e939596a8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""940af298-7139-4b49-a833-07bc9b113ddd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player Movement
        m_PlayerMovement = asset.FindActionMap("Player Movement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovement_Rotation = m_PlayerMovement.FindAction("Rotation", throwIfNotFound: true);
        m_PlayerMovement_Run = m_PlayerMovement.FindAction("Run", throwIfNotFound: true);
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_Attack = m_Actions.FindAction("Attack", throwIfNotFound: true);
        m_Actions_Dash = m_Actions.FindAction("Dash", throwIfNotFound: true);
        m_Actions_Block = m_Actions.FindAction("Block", throwIfNotFound: true);
        m_Actions_FocusOnEnemy = m_Actions.FindAction("Focus On Enemy", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player Movement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Movement;
    private readonly InputAction m_PlayerMovement_Rotation;
    private readonly InputAction m_PlayerMovement_Run;
    public struct PlayerMovementActions
    {
        private @PlayerInputScheme m_Wrapper;
        public PlayerMovementActions(@PlayerInputScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputAction @Rotation => m_Wrapper.m_PlayerMovement_Rotation;
        public InputAction @Run => m_Wrapper.m_PlayerMovement_Run;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Rotation.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnRotation;
                @Run.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnRun;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // Actions
    private readonly InputActionMap m_Actions;
    private IActionsActions m_ActionsActionsCallbackInterface;
    private readonly InputAction m_Actions_Attack;
    private readonly InputAction m_Actions_Dash;
    private readonly InputAction m_Actions_Block;
    private readonly InputAction m_Actions_FocusOnEnemy;
    public struct ActionsActions
    {
        private @PlayerInputScheme m_Wrapper;
        public ActionsActions(@PlayerInputScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Actions_Attack;
        public InputAction @Dash => m_Wrapper.m_Actions_Dash;
        public InputAction @Block => m_Wrapper.m_Actions_Block;
        public InputAction @FocusOnEnemy => m_Wrapper.m_Actions_FocusOnEnemy;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnAttack;
                @Dash.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDash;
                @Block.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnBlock;
                @Block.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnBlock;
                @Block.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnBlock;
                @FocusOnEnemy.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnFocusOnEnemy;
                @FocusOnEnemy.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnFocusOnEnemy;
                @FocusOnEnemy.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnFocusOnEnemy;
            }
            m_Wrapper.m_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Block.started += instance.OnBlock;
                @Block.performed += instance.OnBlock;
                @Block.canceled += instance.OnBlock;
                @FocusOnEnemy.started += instance.OnFocusOnEnemy;
                @FocusOnEnemy.performed += instance.OnFocusOnEnemy;
                @FocusOnEnemy.canceled += instance.OnFocusOnEnemy;
            }
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
    }
    public interface IActionsActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnBlock(InputAction.CallbackContext context);
        void OnFocusOnEnemy(InputAction.CallbackContext context);
    }
}
