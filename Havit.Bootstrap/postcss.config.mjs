// Mirrors the upstream Bootstrap 6 build (build/postcss.config.mjs):
// CSS custom properties are written unprefixed in the Sass sources and
// prefixed with "bs-" here (replaces the former $prefix Sass variable).
import postcssPrefixCustomProperties from 'postcss-prefix-custom-properties'
import autoprefixer from 'autoprefixer'

// HAVIT workaround for the Bootstrap 6 alpha layering gap: _transitions.scss emits its rules
// (.fade*, .collapse*, .collapsing*) OUTSIDE any @layer, while all utilities live in @layer
// utilities. Unlayered styles beat layered ones, so the long-standing idiom of overriding
// .collapse visibility with responsive display utilities (e.g. "collapse md:d-flex" in
// HxSidebar) silently stops working. Until upstream layers _transitions.scss, move those
// rules into @layer components, restoring the documented utility-over-component cascade.
// (Remove when https://github.com/twbs/bootstrap _transitions.scss gains a layer upstream.)
const layerTransitionRules = {
  postcssPlugin: 'havit-layer-transition-rules',
  OnceExit (root, { AtRule }) {
    const selectorPattern = /^\s*\.(fade|collapse|collapsing)([.:,\s[]|$)/
    root.each(node => {
      if (node.type !== 'rule' || node.parent.type !== 'root') {
        return
      }
      if (node.selectors.every(s => selectorPattern.test(s))) {
        const layer = new AtRule({ name: 'layer', params: 'components' })
        node.replaceWith(layer)
        layer.append(node)
      }
    })
  }
}

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
      autoprefixer({ cascade: false }),
      layerTransitionRules
    ]
  }
}
