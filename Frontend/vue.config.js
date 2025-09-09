const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true
})
module.exports = {
  chainWebpack: config => {
    config
      .plugin('define')
      .tap(args => {
        args[0]['__VUE_PROD_HYDRATION_MISMATCH_DETAILS__'] = true
        // 添加其他可能缺失的标志
        args[0]['__VUE_OPTIONS_API__'] = true
        args[0]['__VUE_PROD_DEVTOOLS__'] = false
        return args
      })
  }
}
module.exports = {
  devServer: {
    proxy: {
      '/api': {
        target: 'http://localhost:5037',
        changeOrigin: true,
        pathRewrite: { '^/api': '' }
      }
    }
  }
}