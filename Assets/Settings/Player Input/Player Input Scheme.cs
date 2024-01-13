//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
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

public partial class @PlayerInputScheme: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputScheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input Scheme"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""6aa0d5c0-4475-4e5f-b282-08cbf452caa3"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d0fc48a8-5002-4561-b779-7b30064f1630"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
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
                    ""id"": ""6dbc22b1-5bea-4d82-8d4d-8230284d2037"",
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
                    ""id"": ""696724e9-b83f-4e5c-88b4-5f10d69dc398"",
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
                    ""name"": ""Focus"",
                    ""type"": ""Button"",
                    ""id"": ""e004e46e-4011-43d7-bf5b-b525bc2aa54f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move Focus Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5326e22a-c2da-4ede-ba23-e9ea8485346d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Nearest Spawn"",
                    ""type"": ""Button"",
                    ""id"": ""2fc6cf32-fbb6-4198-a07f-76fb6441b82f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Random Spawn"",
                    ""type"": ""Button"",
                    ""id"": ""6d7dfd31-7af9-4e53-bc07-fe6f3ed88905"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""31261c97-2bdd-4554-a714-2fc125573e77"",
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
                    ""action"": ""Focus"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""b57073f0-8892-4aa7-918c-ab3ba4e34061"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Focus Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c7599e2-45a8-4b31-ade1-edecbd07a151"",
                    ""path"": ""<Keyboard>/n"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nearest Spawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d1c05e6-336c-452f-9512-c85d9ed3a5e1"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Random Spawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0c756a1-e43c-431d-b926-7d80a88fc7c4"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Quality Change"",
            ""id"": ""dd3c67f4-f3cd-4ff7-b47b-f4c715b07c99"",
            ""actions"": [
                {
                    ""name"": ""Low Quality"",
                    ""type"": ""Button"",
                    ""id"": ""54b902a6-6ba2-4f8a-b58e-193901e88c00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Medium Quality"",
                    ""type"": ""Button"",
                    ""id"": ""da505136-9839-4e23-bac8-93255eeec4d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""High Quality"",
                    ""type"": ""Button"",
                    ""id"": ""f9088d6c-2e56-4fed-a48f-c1885e55b708"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ultra Quality"",
                    ""type"": ""Button"",
                    ""id"": ""5116ce2f-4aa1-4f8f-9d15-8a633f46de1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8b413534-4a30-42af-8fca-928398f329da"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Low Quality"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e75430b-30b4-4dd0-9fba-9ff8655329a3"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Medium Quality"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""676c6324-5fc4-46be-8bf2-3f8fa0bfacc3"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""High Quality"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cbb4f6d7-0363-4351-b82e-1c972d049d9c"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ultra Quality"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_Movement = m_Controls.FindAction("Movement", throwIfNotFound: true);
        m_Controls_Rotation = m_Controls.FindAction("Rotation", throwIfNotFound: true);
        m_Controls_Run = m_Controls.FindAction("Run", throwIfNotFound: true);
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_Attack = m_Actions.FindAction("Attack", throwIfNotFound: true);
        m_Actions_Dash = m_Actions.FindAction("Dash", throwIfNotFound: true);
        m_Actions_Block = m_Actions.FindAction("Block", throwIfNotFound: true);
        m_Actions_Focus = m_Actions.FindAction("Focus", throwIfNotFound: true);
        m_Actions_MoveFocusPoint = m_Actions.FindAction("Move Focus Point", throwIfNotFound: true);
        m_Actions_NearestSpawn = m_Actions.FindAction("Nearest Spawn", throwIfNotFound: true);
        m_Actions_RandomSpawn = m_Actions.FindAction("Random Spawn", throwIfNotFound: true);
        m_Actions_Restart = m_Actions.FindAction("Restart", throwIfNotFound: true);
        // Quality Change
        m_QualityChange = asset.FindActionMap("Quality Change", throwIfNotFound: true);
        m_QualityChange_LowQuality = m_QualityChange.FindAction("Low Quality", throwIfNotFound: true);
        m_QualityChange_MediumQuality = m_QualityChange.FindAction("Medium Quality", throwIfNotFound: true);
        m_QualityChange_HighQuality = m_QualityChange.FindAction("High Quality", throwIfNotFound: true);
        m_QualityChange_UltraQuality = m_QualityChange.FindAction("Ultra Quality", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private List<IControlsActions> m_ControlsActionsCallbackInterfaces = new List<IControlsActions>();
    private readonly InputAction m_Controls_Movement;
    private readonly InputAction m_Controls_Rotation;
    private readonly InputAction m_Controls_Run;
    public struct ControlsActions
    {
        private @PlayerInputScheme m_Wrapper;
        public ControlsActions(@PlayerInputScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Controls_Movement;
        public InputAction @Rotation => m_Wrapper.m_Controls_Rotation;
        public InputAction @Run => m_Wrapper.m_Controls_Run;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void AddCallbacks(IControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_ControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ControlsActionsCallbackInterfaces.Add(instance);
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

        private void UnregisterCallbacks(IControlsActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Rotation.started -= instance.OnRotation;
            @Rotation.performed -= instance.OnRotation;
            @Rotation.canceled -= instance.OnRotation;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
        }

        public void RemoveCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_ControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);

    // Actions
    private readonly InputActionMap m_Actions;
    private List<IActionsActions> m_ActionsActionsCallbackInterfaces = new List<IActionsActions>();
    private readonly InputAction m_Actions_Attack;
    private readonly InputAction m_Actions_Dash;
    private readonly InputAction m_Actions_Block;
    private readonly InputAction m_Actions_Focus;
    private readonly InputAction m_Actions_MoveFocusPoint;
    private readonly InputAction m_Actions_NearestSpawn;
    private readonly InputAction m_Actions_RandomSpawn;
    private readonly InputAction m_Actions_Restart;
    public struct ActionsActions
    {
        private @PlayerInputScheme m_Wrapper;
        public ActionsActions(@PlayerInputScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Actions_Attack;
        public InputAction @Dash => m_Wrapper.m_Actions_Dash;
        public InputAction @Block => m_Wrapper.m_Actions_Block;
        public InputAction @Focus => m_Wrapper.m_Actions_Focus;
        public InputAction @MoveFocusPoint => m_Wrapper.m_Actions_MoveFocusPoint;
        public InputAction @NearestSpawn => m_Wrapper.m_Actions_NearestSpawn;
        public InputAction @RandomSpawn => m_Wrapper.m_Actions_RandomSpawn;
        public InputAction @Restart => m_Wrapper.m_Actions_Restart;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void AddCallbacks(IActionsActions instance)
        {
            if (instance == null || m_Wrapper.m_ActionsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ActionsActionsCallbackInterfaces.Add(instance);
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Block.started += instance.OnBlock;
            @Block.performed += instance.OnBlock;
            @Block.canceled += instance.OnBlock;
            @Focus.started += instance.OnFocus;
            @Focus.performed += instance.OnFocus;
            @Focus.canceled += instance.OnFocus;
            @MoveFocusPoint.started += instance.OnMoveFocusPoint;
            @MoveFocusPoint.performed += instance.OnMoveFocusPoint;
            @MoveFocusPoint.canceled += instance.OnMoveFocusPoint;
            @NearestSpawn.started += instance.OnNearestSpawn;
            @NearestSpawn.performed += instance.OnNearestSpawn;
            @NearestSpawn.canceled += instance.OnNearestSpawn;
            @RandomSpawn.started += instance.OnRandomSpawn;
            @RandomSpawn.performed += instance.OnRandomSpawn;
            @RandomSpawn.canceled += instance.OnRandomSpawn;
            @Restart.started += instance.OnRestart;
            @Restart.performed += instance.OnRestart;
            @Restart.canceled += instance.OnRestart;
        }

        private void UnregisterCallbacks(IActionsActions instance)
        {
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Block.started -= instance.OnBlock;
            @Block.performed -= instance.OnBlock;
            @Block.canceled -= instance.OnBlock;
            @Focus.started -= instance.OnFocus;
            @Focus.performed -= instance.OnFocus;
            @Focus.canceled -= instance.OnFocus;
            @MoveFocusPoint.started -= instance.OnMoveFocusPoint;
            @MoveFocusPoint.performed -= instance.OnMoveFocusPoint;
            @MoveFocusPoint.canceled -= instance.OnMoveFocusPoint;
            @NearestSpawn.started -= instance.OnNearestSpawn;
            @NearestSpawn.performed -= instance.OnNearestSpawn;
            @NearestSpawn.canceled -= instance.OnNearestSpawn;
            @RandomSpawn.started -= instance.OnRandomSpawn;
            @RandomSpawn.performed -= instance.OnRandomSpawn;
            @RandomSpawn.canceled -= instance.OnRandomSpawn;
            @Restart.started -= instance.OnRestart;
            @Restart.performed -= instance.OnRestart;
            @Restart.canceled -= instance.OnRestart;
        }

        public void RemoveCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IActionsActions instance)
        {
            foreach (var item in m_Wrapper.m_ActionsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ActionsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);

    // Quality Change
    private readonly InputActionMap m_QualityChange;
    private List<IQualityChangeActions> m_QualityChangeActionsCallbackInterfaces = new List<IQualityChangeActions>();
    private readonly InputAction m_QualityChange_LowQuality;
    private readonly InputAction m_QualityChange_MediumQuality;
    private readonly InputAction m_QualityChange_HighQuality;
    private readonly InputAction m_QualityChange_UltraQuality;
    public struct QualityChangeActions
    {
        private @PlayerInputScheme m_Wrapper;
        public QualityChangeActions(@PlayerInputScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @LowQuality => m_Wrapper.m_QualityChange_LowQuality;
        public InputAction @MediumQuality => m_Wrapper.m_QualityChange_MediumQuality;
        public InputAction @HighQuality => m_Wrapper.m_QualityChange_HighQuality;
        public InputAction @UltraQuality => m_Wrapper.m_QualityChange_UltraQuality;
        public InputActionMap Get() { return m_Wrapper.m_QualityChange; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(QualityChangeActions set) { return set.Get(); }
        public void AddCallbacks(IQualityChangeActions instance)
        {
            if (instance == null || m_Wrapper.m_QualityChangeActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_QualityChangeActionsCallbackInterfaces.Add(instance);
            @LowQuality.started += instance.OnLowQuality;
            @LowQuality.performed += instance.OnLowQuality;
            @LowQuality.canceled += instance.OnLowQuality;
            @MediumQuality.started += instance.OnMediumQuality;
            @MediumQuality.performed += instance.OnMediumQuality;
            @MediumQuality.canceled += instance.OnMediumQuality;
            @HighQuality.started += instance.OnHighQuality;
            @HighQuality.performed += instance.OnHighQuality;
            @HighQuality.canceled += instance.OnHighQuality;
            @UltraQuality.started += instance.OnUltraQuality;
            @UltraQuality.performed += instance.OnUltraQuality;
            @UltraQuality.canceled += instance.OnUltraQuality;
        }

        private void UnregisterCallbacks(IQualityChangeActions instance)
        {
            @LowQuality.started -= instance.OnLowQuality;
            @LowQuality.performed -= instance.OnLowQuality;
            @LowQuality.canceled -= instance.OnLowQuality;
            @MediumQuality.started -= instance.OnMediumQuality;
            @MediumQuality.performed -= instance.OnMediumQuality;
            @MediumQuality.canceled -= instance.OnMediumQuality;
            @HighQuality.started -= instance.OnHighQuality;
            @HighQuality.performed -= instance.OnHighQuality;
            @HighQuality.canceled -= instance.OnHighQuality;
            @UltraQuality.started -= instance.OnUltraQuality;
            @UltraQuality.performed -= instance.OnUltraQuality;
            @UltraQuality.canceled -= instance.OnUltraQuality;
        }

        public void RemoveCallbacks(IQualityChangeActions instance)
        {
            if (m_Wrapper.m_QualityChangeActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IQualityChangeActions instance)
        {
            foreach (var item in m_Wrapper.m_QualityChangeActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_QualityChangeActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public QualityChangeActions @QualityChange => new QualityChangeActions(this);
    public interface IControlsActions
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
        void OnFocus(InputAction.CallbackContext context);
        void OnMoveFocusPoint(InputAction.CallbackContext context);
        void OnNearestSpawn(InputAction.CallbackContext context);
        void OnRandomSpawn(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
    }
    public interface IQualityChangeActions
    {
        void OnLowQuality(InputAction.CallbackContext context);
        void OnMediumQuality(InputAction.CallbackContext context);
        void OnHighQuality(InputAction.CallbackContext context);
        void OnUltraQuality(InputAction.CallbackContext context);
    }
}
