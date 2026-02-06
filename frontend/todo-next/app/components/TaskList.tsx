import { TaskItem } from "./TaskItem";
function TaskList() {
  return (
    <div className="">
      <ul className="mt-10 w-[500px] divide-y divide-purple-300">
        <TaskItem title="task 1" />
        <TaskItem title="task 2" />
        <TaskItem title="task 3" />
      </ul>
    </div>
  );
}

export default TaskList;
