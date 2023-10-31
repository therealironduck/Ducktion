import { defineConfig } from 'vitepress'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  title: "Ducktion Documentation",
  description: "A simple, flexible dependency injection solution for Unity",
  themeConfig: {
    // https://vitepress.dev/reference/default-theme-config
    nav: [
      { text: 'Home', link: '/' },
      { text: 'Documentation', link: '/getting-started' }
    ],

    sidebar: [
      {
        text: 'Basics',
        items: [
          { text: 'Getting started', link: '/getting-started' },
          { text: 'Installation', link: '/installation' }
        ]
      },
      {
        text: 'Samples',
        items: [
          { text: 'Getting started', link: '/getting-started' } 
        ]
      }
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/therealironduck/ducktion' }
    ]
  }
})
