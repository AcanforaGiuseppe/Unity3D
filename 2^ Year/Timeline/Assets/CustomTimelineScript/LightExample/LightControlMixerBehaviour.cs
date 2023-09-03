using UnityEngine;
using UnityEngine.Playables;

public class LightControlMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Light trackBinding = playerData as Light;
        float finalIntensity = 0f;
        Color finalColor = Color.black;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount(); // Get the number of all clips on this track

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<LightControlBehaviour> inputPlayable = (ScriptPlayable<LightControlBehaviour>)playable.GetInput(i);
            LightControlBehaviour input = inputPlayable.GetBehaviour();

            // Use the above variables to process each frame of this playable.
            finalIntensity += input.intensity * inputWeight;
            finalColor += input.color * inputWeight;
        }

        // Assign the result to the bound object
        trackBinding.intensity = finalIntensity;
        trackBinding.color = finalColor;
    }

}