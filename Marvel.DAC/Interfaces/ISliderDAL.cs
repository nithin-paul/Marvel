using Marvel.Model;
using System.Collections.Generic;

namespace Marvel.DAC.Interfaces
{
    public interface ISliderDAL
    {
        SliderDTO GetSlider(int id);

        SliderDTO AddOrUpdateSlider(int? Id, SliderDTO slider);

        List<SliderDTO> GetSliders();

        bool DeleteSlider(int id);
    }
}
