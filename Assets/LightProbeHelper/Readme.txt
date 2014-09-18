SETTING UP LIGHT PROBE GENERATOR
1.) First, create your light probe group (GameObject -> Create Empty, then Component -> Rendering -> Light Probe Group)
2.) Attach the LightProbeGenerator component (Component -> Light Probe Helper -> Light Probe Generator)
3.) Set how you want the placement algorithm to work (Grid or Random)

GENERATING LIGHT PROBES
1.) ALWAYS remember to delete light probes before generating new ones (Select All -> Delete Selected)
2.) Set the volumes you want the light probes to occupy (as well as either subdivisions or the number of light probes within the volume, depending on the selected placement algorithm).
3.) Hit 'Generate'
4.) Bake your lightmap