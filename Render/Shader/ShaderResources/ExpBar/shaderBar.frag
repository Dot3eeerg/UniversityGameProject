#version 410 core

in vec2 frag_texCoords;

out vec4 out_color;

uniform sampler2D uTexture;
uniform float uFilled;

void main() {
    float kek = frag_texCoords.x > uFilled ? 0.0 : 0.5;
    vec2 anime = vec2(frag_texCoords.x / 2 + kek, frag_texCoords.y);
    vec4 mango = texture(uTexture, anime);
    if (mango.a < 0.15)
        discard;
    out_color = texture(uTexture, anime);
}
