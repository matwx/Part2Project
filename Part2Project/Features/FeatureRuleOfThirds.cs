using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.ImageSegmentation;
using Part2Project.Saliency;

namespace Part2Project.Features
{
    class FeatureRuleOfThirds : IFeature
    {
        public double ComputeFeature(Bitmap image)
        {
            // Get saliency segmentation
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 150, 0.8);
            SaliencySegmentation ss = new SaliencySegmentation(s, image, 0.8);

            return 0.0;
        }
    }
}
