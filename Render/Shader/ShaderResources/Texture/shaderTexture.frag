#version 410 core

in vec2 frag_texCoords;

out vec4 out_color;

uniform sampler2D uTexture;

void main() {
    vec4 kek = texture(uTexture, frag_texCoords);
    if (kek.a < 0.15)
            discard;
    out_color = kek;
}
