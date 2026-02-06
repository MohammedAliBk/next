import AddTaskForm from "./components/AddTaskForm";
import SearchBar from "./components/SearchBar";
import TaskList from "./components/TaskList";

export default function Home() {
  return (
    <div >
      <main className="min-h-screen bg-gray-100 flex flex-col items-center">
        <>
        <h1 className=" text-2xl font-bold mt-10 tracking-wide">Todo Next</h1>
        <SearchBar/>
        <TaskList/>
        <AddTaskForm/>
        </>
      </main>
    </div>
  );
}
