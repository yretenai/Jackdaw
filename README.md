# Jackdaw

Carbon Engine (EVE Online) Research

### Engine Architecture

The engine is constructed from several different components mostly named after colors, this is my best guess what they all mean.

- Blue - Python scripting interface.
- Black - Serialized data. (sits on top of blue)
- Red - Serialized scene descriptor (see SOF, replaced by black)
- SOF - [Space Object Factory](https://www.eveonline.com/news/view/one-file-to-rule-them-all), a scene/3D resource template. (merged into red)
- Trinity - 3d renderer, (the [new PBR](https://www.eveonline.com/news/view/pbr-and-making-eve-look-real) and [texture](https://www.eveonline.com/news/view/the-many-additions-of-v5plusplus) system.)
