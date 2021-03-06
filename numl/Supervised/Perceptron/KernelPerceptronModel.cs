using System;
using System.Linq;
using numl.Math.Kernels;
using numl.Math.LinearAlgebra;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using numl.Model;
using numl.Utils;

namespace numl.Supervised.Perceptron
{
    [Serializable]
    public class KernelPerceptronModel : Model
    {
        public IKernel Kernel { get; set; }
        public Vector Y { get; set; }
        public Vector A { get; set; }
        public Matrix X { get; set; }

        public override double Predict(Vector y)
        {
            var K = Kernel.Project(X, y);
            double v = 0;
            for (int i = 0; i < A.Length; i++)
                v += A[i] * Y[i] * K[i];

            return v;
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Kernel", Kernel.GetType().Name);
            Xml.Write<Descriptor>(writer, Descriptor);
            Xml.Write<Vector>(writer, Y);
            Xml.Write<Vector>(writer, A);
            Xml.Write<Matrix>(writer, X);
        }

        public override void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            var type = Ject.FindType(reader.GetAttribute("Kernel"));
            Kernel = (IKernel)Activator.CreateInstance(type);
            reader.ReadStartElement();

            Descriptor = Xml.Read<Descriptor>(reader);
            Y = Xml.Read<Vector>(reader);
            A = Xml.Read<Vector>(reader);
            X = Xml.Read<Matrix>(reader);
        }
    }
}
