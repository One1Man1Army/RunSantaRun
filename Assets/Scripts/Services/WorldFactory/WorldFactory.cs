using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSR.ServicesLogic
{
    public sealed class WorldFactory : IWorldFactory
    {
        private readonly IAssetsProvider _assetsProvider;
        private readonly IInputProvider _inputProvider;

        public WorldFactory(IAssetsProvider assetsProvider, IInputProvider inputProvider) 
        {
            _assetsProvider = assetsProvider;
            _inputProvider = inputProvider;
        }
    }
}
