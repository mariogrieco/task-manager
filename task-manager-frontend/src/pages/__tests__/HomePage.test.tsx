import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import HomePage from '../HomePage'

describe('HomePage', () => {
  it('renders welcome message', () => {
    render(<HomePage />)
    expect(screen.getByText(/Welcome to the Task Manager application/i)).toBeInTheDocument()
  })
})
