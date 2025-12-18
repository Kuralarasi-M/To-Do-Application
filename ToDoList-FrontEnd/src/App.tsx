
import { Route, Routes } from 'react-router-dom'
import './App.css'
import Login from './components/Login'
import ToDo from './components/ToDo'

import NavBar from './components/NavBar'

function App() {
 
  

  return (
    <>
      {/* <ToDo></ToDo> */}
      <NavBar/>
      <Routes>
        <Route path="/" element={<Login />}/>
        <Route path="/dashboard" element={<ToDo />}/>
      </Routes>
    
    </>
  )
}

export default App
