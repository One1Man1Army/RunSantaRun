using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.ServicesLogic
{
    public sealed class WorldBuilder : IWorldBuilder
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;

        public WorldBuilder(IAssetsProvider assetsProvider, IInputProvider inputProvider) 
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
        }
    }
}
