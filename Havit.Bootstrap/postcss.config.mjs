// Mirrors the upstream Bootstrap 6 build (build/postcss.config.mjs):
// CSS custom properties are written unprefixed in the Sass sources and
// prefixed with "bs-" here (replaces the former $prefix Sass variable).
import postcssPrefixCustomProperties from 'postcss-prefix-custom-properties'
import autoprefixer from 'autoprefixer'

const mapConfig = {
  inline: false,
  annotation: true,
  sourcesContent: true
}

export default () => {
  return {
    map: mapConfig,
    plugins: [
      postcssPrefixCustomProperties({
        prefix: 'bs-',
        ignore: [/^--bs-/]
      }),
      autoprefixer({ cascade: false })
    ]
  }
}
