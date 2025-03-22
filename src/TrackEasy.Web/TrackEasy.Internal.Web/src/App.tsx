import { Button } from "@/components/ui/button"

function App() {
  function testClick() {
    const a = 5;
    const b = 5;
    const c = a + b;
    console.log(c);
  }
  
  return (
    <div className="flex flex-col items-center justify-center min-h-svh">
      <Button className="cursor-pointer" onClick={testClick}>Click me</Button>
    </div>
  )
}

export default App
