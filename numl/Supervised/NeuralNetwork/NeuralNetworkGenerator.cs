﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using numl.Math.LinearAlgebra;
using System.Xml;
using numl.Math.Functions;

namespace numl.Supervised.NeuralNetwork
{
    public class NeuralNetworkGenerator : Generator
    {
        public double LearningRate { get; set; }
        public int MaxIterations { get; set; }
        public IFunction Activation { get; set; }

        public NeuralNetworkGenerator()
        {
            LearningRate = 0.9;
            MaxIterations = -1;
            Activation = new Tanh();
        }

        public override IModel Generate(Matrix x, Vector y)
        {
            // because I said so...
            if (MaxIterations == -1) MaxIterations = x.Rows * 100;

            var network = Network.Default(Descriptor, x, y, Activation);

            for (int i = 0; i < MaxIterations; i++)
            {
                network.Forward(x[i % x.Rows, VectorType.Row]);
                network.Back(y[i % y.Length], LearningRate);
            }

            return new NeuralNetworkModel { Descriptor = Descriptor, Network = network };
        }
    }
}