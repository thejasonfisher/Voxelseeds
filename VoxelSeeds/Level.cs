﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelSeeds
{
    /// <summary>
    /// A level creates and maintains a map and the automaton for the living
    /// parts.
    /// 
    /// The level is a base class. To define a spezific level create a child
    /// class.
    /// </summary>
    abstract class Level
    {
        public int TargetBiomass { get { return _targetBiomass; } protected set { _targetBiomass = value; } }
        public int CurrentBiomass { get { return _currentBiomass; } protected set { _currentBiomass = value; } }
        public int ParasiteBiomass { get { return _currentParasiteMass; } protected set { _currentParasiteMass = value; } }
        public int FinalParasiteBiomass { get { return _finalParasiteMass; } protected set { _finalParasiteMass = value; } }

        public Level()
        {
            _numRemainingSeeds = new int[Enum.GetValues(typeof(VoxelType)).Length];

            Initialize();
        }

        /// <summary>
        /// Fill all variables and the automaton with meaningful variables.
        /// It is not necessary to allocate the _numRemainingSeeds array
        /// just fill it.
        /// </summary>
        public abstract void Initialize();

        public bool IsVictory()    { return _currentBiomass >= _targetBiomass; }
        public bool IsLost() { return _currentParasiteMass >= _finalParasiteMass; }

        public Map GetMap() { return _automaton.Map; }
        public void InsertSeed(int x, int y, int z, VoxelType type) { return _automaton.InsertSeed(x, y, z, type); }

        public void Tick(Action<IEnumerable<Voxel>, IEnumerable<Voxel>> updateInstanceData)
        {
            int newBiomass;
            int newParasites;
            _automaton.Tick(ref updateInstanceData, out newBiomass, out newParasites);
            _currentBiomass += newBiomass;
            _currentParasiteMass += newParasites;

            Debug.Assert( _currentBiomass >= 0 );
            Debug.Assert(_currentParasiteMass >= 0);
        }

        protected Automaton _automaton;
        protected int _targetBiomass;
        protected int _currentBiomass;
        protected int _currentParasiteMass;
        protected int _finalParasiteMass;

        protected int[] _numRemainingSeeds;
    }
}
