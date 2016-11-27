using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Part2Project.ImageSegmentation;

namespace Part2Project.Features
{
    class FeatureRuleOfThirds : IFeature
    {
        public double ComputeFeature(Bitmap image)
        {
            // Get saliency segmentation
            Segmentation s = GraphBasedImageSegmentation.Segment(image, 150, 0.8);
            RuleOfThirdsSegmentation rots = new RuleOfThirdsSegmentation(s, image, 0.8);

            // Compute the scaling factor
            double factor = 0;
            for (int i = 0; i < rots.NumSegments; i++)
            {
                factor += rots.GetSegmentsSaliency(i)*rots.GetSegmentsSize(i);
            }

            double result = 0;
            const double sigma = 0.17 * 4.0;
            for (int i = 0; i < rots.NumSegments; i++)
            {
                result += rots.GetSegmentsSaliency(i) * rots.GetSegmentsSize(i) * Math.Exp(-rots.GetSegmentsDistance(i)*rots.GetSegmentsDistance(i)/(2*sigma));
            }

            result /= factor;

            return result;
        }
    }
}
