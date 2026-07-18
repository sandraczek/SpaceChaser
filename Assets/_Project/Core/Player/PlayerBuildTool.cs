using System;
using SpaceChaser.Core.Building;
using SpaceChaser.Core.Inputs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SpaceChaser.Core.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerBuildTool : MonoBehaviour
    {
        private PlayerController _controller;
        private IBuildService _buildService;
        private IInputReader _inputs;

        //DEBUG
        [SerializeField] private BuildData _build;


        [Inject]
        public void Construct(IBuildService buildService, IInputReader inputs)
        {
            _buildService = buildService;
            _inputs = inputs;
        }

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
        }
        public void OnEnable()
        {
            _inputs.OnPrimaryActionPressed += HandlePrimaryAction;
        }
        public void OnDisable()
        {
            _inputs.OnPrimaryActionPressed -= HandlePrimaryAction;
        }

        private void HandlePrimaryAction()
        {
            _buildService.Build(_controller.GetWorldAimPosition(), _build);
        }

    }
}