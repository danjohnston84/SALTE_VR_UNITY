SALTE is a listening test software created to aid future research and development of spatial audio systems. The tool consists of a [virtual reality interface](https://github.com/AudioLabYork/SALTE-VR-interface)  and a dedicated [audio rendering engine](https://github.com/AudioLabYork/SALTE-audio-renderer) for conducting spatial audio listening experiments.

Spatial Audio Listening Test Environment (SALTE) can be used in the assessment of:

- Spatial audio recording techniques,
- Spatial audio codecs,
- Binaural rendering algorithms,
- Head-Related Transfer Function (HRTF) datasets,
- Virtual soundscapes and room acoustics.

**UNITY VR Interface Features**

- Build a listening test environment within the audio engine. VR environment is automatically built based on these inputs.
- Direct communication with audio engine via OSC for UI building, auditory stimuli manipulation and trial selection.
- Built-in flexible listening test interface accommodating standard test paradigms: ITU-R BS.1116, ITU-R BS.1534 (&quot;MUSHRA&quot;) and 3GPP TS26.259.

**Getting Started**

- Download Unity Project
- Open main environment scene
- Define OSC IP address and ports in OSC Handler Object (default parameters are in place)
- Test UI Building on OSC Handler Object without the need for OSC messages
- Run listening test in Unity Editor or build for standalone.
- Download [audio render engine](https://github.com/AudioLabYork/SALTE-audio-renderer) and follow instructions for setting up test environment.

**Authors**

- Daniel Johnston, email: [dij502@york.ac.uk](mailto:dij502@york.ac.uk)
- Benjamin Tsui, email: bt712@york.ac.uk

**License**

GNU General Public License v3.0

**Safety Note**

SALTE is an experimental tool which is in its early stage of development and may contain bugs. Please be aware that it may break anytime during runtime and users might experience unpleasant and loud noises.
