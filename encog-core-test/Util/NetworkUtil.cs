//
// Encog(tm) Core v3.1 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2012 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Engine.Network.Activation;
using Encog.MathUtil.Randomize;
using Encog.ML.Train;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Encog.Util
{
    using Encog.MathUtil;

    [TestClass]
    public class NetworkUtil
    {
        public static BasicNetwork CreateXORNetworkUntrained()
        {
            // random matrix data.  However, it provides a constant starting point 
            // for the unit tests.		
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 4));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
            network.Structure.FinalizeStructure();

            (new ConsistentRandomizer(-1, 1)).Randomize(network);

            return network;
        }

        public static BasicNetwork CreateXORNetworknNguyenWidrowUntrained()
        {
            // random matrix data.  However, it provides a constant starting point 
            // for the unit tests.

            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(null, true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 3));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 3));
            network.AddLayer(new BasicLayer(null, false, 1));
            network.Structure.FinalizeStructure();
            (new NguyenWidrowRandomizer()).Randomize(network);

            return network;
        }

        public static void TestTraining(IMLTrain train, double requiredImprove)
        {
            train.Iteration();
            double error1 = train.Error;

            for (int i = 0; i < 10; i++)
                train.Iteration();

            double error2 = train.Error;

            double improve = (error1 - error2) / error1;
            Assert.IsTrue(improve >= requiredImprove, "Improve rate too low for " + train.GetType().Name +
                    ",Improve=" + improve + ",Needed=" + requiredImprove);
        }
        [TestMethod]
        public void TestDateNormalizeDaysEncode()
        {
            var eq = new Equilateral(DateTime.DaysInMonth(2012, 1), -1, 1);
            var encoded = eq.Encode(15);
            StringBuilder b = new StringBuilder(encoded.Length);
            for (int i = 0; i < encoded.Length; i++)
            {
                if (i < encoded.Length - 1)
                    b.Append(encoded[i] + ",");
                else b.Append(encoded[i]);
            }
            Console.WriteLine("Encoded 15 to Equilaterable " + b.ToString());
            Assert.IsNotNull(encoded, "Encoded is not null");
            Assert.IsTrue(encoded[14].ToString() == "-0.984250984251476");
            //Now we get the day back..

            int res = eq.Decode(encoded);
            Console.WriteLine("Result decode == " + res);
            Assert.AreEqual(15, res, "Decoded back to 15");
        }
    }
}
