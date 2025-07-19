// src/utils/carousel.js
import { reactive, toRefs, onMounted, onBeforeUnmount } from 'vue'

export function useCarousel(items, autoPlayInterval = 5000) {
  const state = reactive({
    currentSlide: 0,
    slideInterval: null,
    isHovering: false,
    carouselItems: items
  })

  const prevSlide = () => {
    state.currentSlide = (state.currentSlide - 1 + state.carouselItems.length) % state.carouselItems.length
    resetAutoSlide()
  }

  const nextSlide = () => {
    state.currentSlide = (state.currentSlide + 1) % state.carouselItems.length
    resetAutoSlide()
  }

  const goToSlide = (index) => {
    state.currentSlide = index
    resetAutoSlide()
  }

  const startAutoSlide = () => {
    state.slideInterval = setInterval(() => {
      if (!state.isHovering) {
        nextSlide()
      }
    }, autoPlayInterval)
  }

  const stopAutoSlide = () => {
    if (state.slideInterval) {
      clearInterval(state.slideInterval)
      state.slideInterval = null
    }
  }

  const resetAutoSlide = () => {
    stopAutoSlide()
    startAutoSlide()
  }

  onMounted(() => {
    startAutoSlide()
  })

  onBeforeUnmount(() => {
    stopAutoSlide()
  })

  return {
    ...toRefs(state),
    prevSlide,
    nextSlide,
    goToSlide
  }
}