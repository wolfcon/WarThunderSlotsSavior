//
//  Git.swift
//  WarThunderProfileSavior
//
//  Created by Frank on 2023/3/20.
//

import Foundation

class Git {
    static let extensionName = ".git"
    
    /// Git repository path
    var path: String
    
    var currentBranch: String {
        let output = execute("branch", "--show-current")
        guard output.code == 0 else { return "main" }
        
        return output.message
    }
    
    func addRemote(_ remote: Remote) {
        execute("remote", "add", remote.name, remote.url.absoluteString)
    }
    
    let standardPathCommand: [String]
    
    init(_ path: String) {
        self.path = path
        standardPathCommand = ["-C", path]
    }
    
    @discardableResult
    func execute(_ command: String...) -> Output {
        execute(command: command)
    }
    @discardableResult
    func execute(command: [String]) -> Output {
        var params = standardPathCommand
        params.append(contentsOf: command)
        return Git.execute(params)
    }
    
    @discardableResult
    func `init`() -> Output {
        return execute("init")
    }
    
    @discardableResult
    func stageAll() -> Output {
        return execute("add", ".")
    }
    
    var defaultRemoteNames: [String]? {
        let result = execute("remote")
        if result.isFailed { return nil }
        if result.message.isEmpty { return nil }
        
        return result.message
            .components(separatedBy: CharacterSet.newlines)
            .compactMap { $0.isEmpty ? nil : $0 }
    }
    
    @discardableResult
    func commit(_ message: String = "[mac]github.com/wolfcon") -> Output {
        return execute("commit", "-q", "-m", message)
    }
    
    // MARK: 🌈 Remote handler
    @discardableResult
    func fetch() -> Output {
//        guard let remote = remote else { return Output.needAddRemoteError }
//        return execute("fetch", remote.name)
        return execute("fetch")
    }
    
    @discardableResult
    func pull() -> Output {
//        guard let remote = remote else { return Output.needAddRemoteError }
//        fetch()
//        return execute("pull", remote.name, currentBranch)
        return execute("pull")
    }
    
    @discardableResult
    func pullRebase() -> Output {
//        guard let remote = remote else { return Output.needAddRemoteError }
//        fetch()
//        return execute("pull", "--no-commit", "--rebase", remote.name, currentBranch)
        return execute("pull", "--no-commit", "--rebase")
    }
    
    @discardableResult
    func push(_ remoteName: String? = nil) -> Output {
//        guard let remote = remote else { return Output.needAddRemoteError }
//        return execute("push", remote.name)
        guard let remoteName = remoteName else { return execute("push") }
        return execute("push", remoteName)
    }
    
    @discardableResult
    static func execute(_ command: [String]) -> Output {
        return Launcher.execute(launchPath: Launcher.Path.git,
                                command, environment: ["gpg": Launcher.Path.gpg])
    }
    
    struct Remote {
        var url: URL
        var name: String
    }
}

struct Output {
    var message: String
    var code: Int
    
    static func error(_ message: String) -> Output {
        return Output(message: message, code: -1)
    }
    static let needAddRemoteError = error("Need add remote first.")
    static let noGitError = error("Git has not been installed.")
}

extension Output {
    var isFailed: Bool {
        return code != 0
    }
    
    var upToDate: Bool {
        return !isFailed && message.contains("Already up to date.")
    }
    
    var nothingToCommit: Bool {
        return isFailed && (message.contains("nothing to commit, working tree clean") || message.contains("Your branch is up to date with"))
    }
}

class Launcher {
    struct Path {
        static let shell = "/bin/sh"
        static let git = "/usr/bin/git"
        static let gpg = "/usr/local/bin/gpg"
        
        static func find(_ app: String) -> String? {
            let output = execute(["-c", "which \(app)"], environment: ["$PATH": "/usr/bin"])
            guard output.code == 0 else { return nil }
            return output.message
        }
    }
    
    @discardableResult
    static func execute(launchPath: String = Path.shell, _ command: [String], environment: [String: String]? = nil) -> Output {
        let task = Process()
        task.launchPath = launchPath
        task.arguments = command
        task.environment = environment
        
        let pipe = Pipe()
        task.standardOutput = pipe
        task.standardError = pipe
        try? task.run()
        task.waitUntilExit()
        let data = try? pipe.fileHandleForReading.readToEnd()
        var output = Output(message: "", code: Int(task.terminationStatus))
        guard let data = data, let message: String = String(data: data, encoding: .utf8) else { return output }
        
        output.message = message
        
        return output
    }
}
